import { Component, OnInit, SimpleChanges, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Filter } from '../_models/Filter';
import { PostService } from '../_services/Post.service';
import { ToastrService } from 'ngx-toastr';
import { Router, ActivatedRoute } from '@angular/router';
import { PostsListComponent } from '../posts/posts-list/posts-list.component';
import { NgxSpinnerService } from "ngx-spinner";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {
  @ViewChild(PostsListComponent) private postList: PostsListComponent;

  constructor(
    private http: HttpClient, 
    private postService: PostService, 
    private toastr: ToastrService, 
    private route: ActivatedRoute,
    private spinner: NgxSpinnerService
  ) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      data['posts'].result;
      this.spinner.hide();
    });
  }

  formFilter($event: any){
    this.postList.loadPosts($event);
  }

}
