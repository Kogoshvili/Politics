import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { RatingTopicLite } from 'src/app/_models/RatingTopic';
import { Pagination } from 'src/app/_models/Pagination';

@Component({
  selector: 'app-rating-list',
  templateUrl: './rating-list.component.html',
  styleUrls: ['./rating-list.component.css']
})
export class RatingListComponent implements OnInit {
  topics: RatingTopicLite[];
  pagination: Pagination;
  
  constructor(
    private route: ActivatedRoute,
  ) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.topics = data['topics'].result;
      this.pagination = data['topics'].pagination;
    });
  }
}
