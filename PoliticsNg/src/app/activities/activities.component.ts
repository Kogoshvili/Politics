import { Component, OnInit } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { ActivatedRoute, Router } from '@angular/router';
import { Activity, ActivityComment } from '../_models/Activity';
import { AuthService } from '../_services/Auth.service';
import { ActivityService } from '../_services/Activity.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-activities',
  templateUrl: './activities.component.html',
  styleUrls: ['./activities.component.css']
})
export class ActivitiesComponent implements OnInit {
  activity: Activity;
  like: boolean;
  dislike: boolean;
  for: boolean;
  against: boolean;
  showComments: boolean;
  user: any;

  constructor(
    private spinner: NgxSpinnerService,
    private route: ActivatedRoute,
    private authService: AuthService,
    private activityService: ActivityService,
    private toastr: ToastrService,
    private router: Router
  ) { }

  ngOnInit() {
    this.spinner.hide();
    this.showComments = !(this.router.url == '/home');
    this.route.data.subscribe(data => {
      this.activity = data['activity'];
      if(this.authService.loggedIn()){
        this.user = this.authService.decodedToken.nameid
        if(this.activity.likes.likes.includes(parseInt(this.authService.decodedToken.nameid))){
          this.like = true;
        }
        if(this.activity.likes.dislikes.includes(parseInt(this.authService.decodedToken.nameid))){
          this.dislike = true;
        }
      }
      this.against = (this.activity.comments.filter(r => r.side == 'against')).length > 0;
      this.for = (this.activity.comments.filter(r => r.side == 'for')).length > 0;
    });
  }
  
  ngOnChanges(){
    this.ngOnInit()
  }

  likeActivity(){
    if(this.authService.loggedIn()){
      let user = this.authService.decodedToken.nameid;
      this.activityService.likeActivity(user, this.activity.id).subscribe(
        next => {
          if(this.like){
            this.activity.likes.likes = this.activity.likes.likes.filter(obj => obj !== parseInt(user));
            this.like = false;
          }else{
            this.activity.likes.likes.push(parseInt(user));
            this.like = true;
          }
          if(this.dislike){
            this.activity.likes.dislikes = this.activity.likes.dislikes.filter(obj => obj !== parseInt(user));
            this.dislike = false;
          }
        },
        error => {
          this.toastr.error(error);
        }
      );
    }
  }
  
  dislikeActivity(){
    if(this.authService.loggedIn()){
      let user = this.authService.decodedToken.nameid;
      this.activityService.dislikeActivity(user, this.activity.id).subscribe(
        next => {
          if(this.dislike){
            this.activity.likes.dislikes = this.activity.likes.dislikes.filter(obj => obj !== parseInt(user));
            this.dislike = false;
          }else{
            this.activity.likes.dislikes.push(parseInt(user));
            this.dislike = true;
          }
          if(this.like){
            this.activity.likes.likes = this.activity.likes.likes.filter(obj => obj !== parseInt(user));
            this.like = false;
          }
        },
        error => {
          this.toastr.error(error);
        }
      );
    }
  }

  toggelComments()
  {
    this.showComments = !this.showComments;
  }

}
