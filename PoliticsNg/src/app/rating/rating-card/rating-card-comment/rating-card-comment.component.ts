import { Component, OnInit, Input, HostBinding } from '@angular/core';
import { TopicComment } from '../../../_models/RatingTopic';
import { QuillDeltaToHtmlConverter } from 'quill-delta-to-html';
import { AuthService } from 'src/app/_services/Auth.service';
import { RatingService } from 'src/app/_services/Rating.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-rating-card-comment',
  templateUrl: './rating-card-comment.component.html',
  styleUrls: ['./rating-card-comment.component.css']
})
export class RatingCardCommentComponent implements OnInit {
  // @HostBinding("class.class1") a = false;
  @Input() comment: TopicComment;
  converter: any;
  like: boolean;
  
  constructor(    
    private ratingService: RatingService,
    private authService: AuthService,
    private toastr: ToastrService
  ) { }

  ngOnInit() {
    this.converter = new QuillDeltaToHtmlConverter(JSON.parse(this.comment.content).ops, {});
    this.comment.content = this.converter.convert(); 
    if(this.authService.loggedIn()){
      if(this.comment.likes.includes(parseInt(this.authService.decodedToken.nameid))){
        this.like = true;
      }
    }
  }

  likeComment(){
    if(this.authService.loggedIn()){
      this.ratingService.likeComment(this.authService.decodedToken.nameid, this.comment.id).subscribe(
        next => {
          if(this.like){
            this.comment.likes = this.comment.likes.filter(obj => obj !== parseInt(this.authService.decodedToken.nameid));
          }else{
            this.comment.likes.push(parseInt(this.authService.decodedToken.nameid));
          }
          this.like = !this.like;
        },
        error => {
          this.toastr.error(error);
        }
      )
    }
  }

}