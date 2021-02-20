import { Component, OnInit, Input } from '@angular/core';
import { RatingService } from 'src/app/_services/Rating.service';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/_services/Auth.service';
import { TopicComment } from 'src/app/_models/RatingTopic';

@Component({
  selector: 'app-rating-card-commments-list',
  templateUrl: './rating-card-commments-list.component.html',
  styleUrls: ['./rating-card-commments-list.component.css']
})
export class RatingCardCommmentsListComponent implements OnInit {z
  @Input() topicId: any;
  for: boolean;
  against: boolean;
  user: any;
  comments: TopicComment[];

  constructor(
    private authSerice: AuthService,
    private ratingSerice: RatingService,
    private toastr: ToastrService
  ) { }

  ngOnInit() {
    this.comments = [];
    this.ratingSerice.topicComments(this.topicId).subscribe(
      (res) => {
        this.comments = res;
        this.against = (res.filter(r => r.side == 'against')).length > 0;
        this.for = (res.filter(r => r.side == 'for')).length > 0;
      }, error => {
        this.toastr.error(error);
      }
    )
    
    if(this.authSerice.loggedIn()){
      this.user = this.authSerice.decodedToken;
    }
  }

}
