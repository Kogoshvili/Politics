import { Component, OnInit, Input } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { RatingService } from 'src/app/_services/Rating.service';
import { AuthService } from 'src/app/_services/Auth.service';
import { ToastrService } from 'ngx-toastr';
import { ActivityService } from 'src/app/_services/Activity.service';

@Component({
  selector: 'app-activities-create-comment',
  templateUrl: './activities-create-comment.component.html',
  styleUrls: ['./activities-create-comment.component.css']
})
export class ActivitiesCreateCommentComponent implements OnInit {
  @Input() activityId: any;
  user: any;
  commentForm: FormGroup;
  
  constructor(
    private activityService: ActivityService,
    private authService: AuthService,
    private toastr: ToastrService,
    private fb: FormBuilder
  ) { }

  ngOnInit() {
    this.commentForm = this.fb.group({
      content: ['', Validators.required],
      side: ['for', Validators.required]
    });
    this.user = this.authService.decodedToken;
  }

  
  commentActivity(){
    this.activityService.commentActivity(this.authService.decodedToken.nameid, this.activityId, this.commentForm.value).subscribe(
      (next) => {
        this.toastr.success("Success");
        window.location.reload();
      },
      error => {
        this.toastr.error(error);
      }
    )
  }

}
