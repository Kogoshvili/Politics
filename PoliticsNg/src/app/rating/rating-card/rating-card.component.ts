import { Component, OnInit, Input, ViewChild, ElementRef } from '@angular/core';
import { RatingTopic } from 'src/app/_models/RatingTopic';
import { AuthService } from 'src/app/_services/Auth.service';
import { RatingService } from 'src/app/_services/Rating.service';
import { ToastrService } from 'ngx-toastr';
import { trigger, state, style, animate, transition, useAnimation } from '@angular/animations';
import { bounce, slideInDown, slideInUp, fadeInUp, fadeInDown } from 'ng-animate';
@Component({
  selector: 'app-rating-card',
  templateUrl: './rating-card.component.html',
  styleUrls: ['./rating-card.component.css'],
  // animations: [
  //   trigger('bounce', [transition('* => *', useAnimation(fadeInDown))])

  //   // trigger(
  //   //   'inOutAnimation', 
  //   //   [
  //   //     transition(
  //   //       ':enter', 
  //   //       [
  //   //         style({ height: 0, opacity: 0 }),
  //   //         animate('1s ease-out', 
  //   //                 style({ height: 300, opacity: 1 }))
  //   //       ]
  //   //     ),
  //   //     transition(
  //   //       ':leave', 
  //   //       [
  //   //         style({ height: 300, opacity: 1 }),
  //   //         animate('1s ease-in', 
  //   //                 style({ height: 0, opacity: 0 }))
  //   //       ]
  //   //     )
  //   //   ]
  //   // )
  // ]
})
export class RatingCardComponent implements OnInit {
  @Input() topic: RatingTopic;
  @Input() number: number;
  userId: any;
  like: boolean;
  dislike: boolean;
  showComments: boolean;
  bounce: any;

  constructor(
    private ratingService: RatingService,
    private authService: AuthService,
    private toastr: ToastrService,
  ) { }

  ngOnInit() {
    if(this.authService.loggedIn()){
      this.userId = this.authService.decodedToken.nameid;
      if(this.topic.likes.userId.includes(parseInt(this.userId))){
        this.like = true;
      }
    }
  }

  likeTopic(){
    if(this.userId != null){
      this.ratingService.likeTopic(this.userId, this.topic.id).subscribe(
        next => {
          // if(this.like){
          //   this.topic.likes.userId = this.topic.likes.userId.filter(obj => obj !== parseInt(this.userId));
          //   this.topic.likes.sum -= 1;
          // }else{
          //   this.topic.likes.userId.push(parseInt(this.userId));
          //   this.topic.likes.sum += 1;
          // }
          window.location.reload();
          this.like = !this.like;
        },
        error => {
          this.toastr.error(error);
        }
      )
    }
  }

  dislikeTopic(){}
  
  toggelComments(){
    this.showComments = !this.showComments;
  }
}
