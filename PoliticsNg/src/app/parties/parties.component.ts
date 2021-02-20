import { Component, OnInit, ViewChild } from '@angular/core';
import { Router, ActivatedRouteSnapshot, ActivatedRoute } from '@angular/router';
import { PostsListComponent } from '../posts/posts-list/posts-list.component';
import { Provider } from '../_models/Provider';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-parties',
  templateUrl: './parties.component.html',
  styleUrls: ['./parties.component.css']
})
export class PartiesComponent implements OnInit {
  @ViewChild(PostsListComponent) private postList: PostsListComponent;
  name: any;
  provider: Provider;

  constructor(private route: ActivatedRoute, private spinner: NgxSpinnerService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.provider = data['provider'];
      this.spinner.hide();
    });
  }

  formFilter($event: any){
    this.postList.loadPosts($event);
  }

}
