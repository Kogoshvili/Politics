import { Component, OnInit, SimpleChanges, ViewChild, Output, EventEmitter, Input } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Filter } from '../../_models/Filter';
import { PostService } from '../../_services/Post.service';
import { Router, ActivatedRoute } from '@angular/router';
import { Category } from 'src/app/_models/Category';
import { Provider } from 'src/app/_models/Provider';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-posts-filter',
  templateUrl: './posts-filter.component.html',
  styleUrls: ['./posts-filter.component.css']
})
export class PostsFilterComponent implements OnInit {
  @Output() formFilter = new EventEmitter();
  filter: Filter;
  categories: Category[];
  providers: Provider[];

  constructor(private http: HttpClient, private postService: PostService, private toastr: ToastrService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.postService.getCategories().subscribe(
      (res) => {
        this.categories = res;
      },error => {
        this.toastr.error(error);
      }
    )
    this.postService.getProviderss().subscribe(
      (res) => {
        this.providers = res;
      },error => {
        this.toastr.error(error);
      }
    )
    
    this.filter = {} as Filter;
    this.route.params.subscribe(params => {
      this.filter.provider = params.provider ? params.provider : '';
      this.filter.category = params.category ? params.category : '';
    });
  }

  OnChanges(){
    this.formFilter.emit(this.filter);
  }


}
