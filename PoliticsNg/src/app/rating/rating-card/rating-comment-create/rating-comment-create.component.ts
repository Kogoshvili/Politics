import { Component, OnInit, Input } from '@angular/core';
import { AuthService } from 'src/app/_services/Auth.service';
import { RatingService } from 'src/app/_services/Rating.service';
import { ToastrService } from 'ngx-toastr';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-rating-comment-create',
  templateUrl: './rating-comment-create.component.html',
  styleUrls: ['./rating-comment-create.component.css']
})
export class RatingCommentCreateComponent implements OnInit {
  @Input() topicId: any;
  user: any;
  commentForm: FormGroup;

  constructor(
    private ratingService: RatingService,
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

  commentTopic(){
    this.ratingService.commentTopic(this.authService.decodedToken.nameid, this.topicId, this.commentForm.value).subscribe(
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
