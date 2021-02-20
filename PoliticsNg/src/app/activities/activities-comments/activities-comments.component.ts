import { Component, OnInit, Input } from '@angular/core';
import { ActivityComment } from 'src/app/_models/Activity';
import { ActivityService } from 'src/app/_services/Activity.service';
import { AuthService } from 'src/app/_services/Auth.service';
import { ToastrService } from 'ngx-toastr';
import { QuillDeltaToHtmlConverter } from 'quill-delta-to-html';

@Component({
  selector: 'app-activities-comments',
  templateUrl: './activities-comments.component.html',
  styleUrls: ['./activities-comments.component.css']
})
export class ActivitiesCommentsComponent implements OnInit {
  @Input() comment: ActivityComment;
  converter: any;
  like: boolean;

  constructor(
    private activityService: ActivityService,
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
      this.activityService.likeComment(this.authService.decodedToken.nameid, this.comment.id).subscribe(
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
