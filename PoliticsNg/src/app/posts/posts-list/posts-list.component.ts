import { Component, OnInit, Input, SimpleChanges } from '@angular/core';
import { Pagination, PaginatedResult } from 'src/app/_models/Pagination';
import { Post } from 'src/app/_models/Post';
import { AuthService } from 'src/app/_services/Auth.service';
import { PostService } from 'src/app/_services/Post.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Filter } from 'src/app/_models/Filter';
import { NgxSpinnerService } from "ngx-spinner";

@Component({
  selector: 'app-posts-list',
  templateUrl: './posts-list.component.html',
  styleUrls: ['./posts-list.component.css']
})
export class PostsListComponent implements OnInit {
  @Input() filter: Filter;
  posts: Post[];
  pagination: Pagination;


  constructor(
    private authService: AuthService,
    private postService: PostService,
    private route: ActivatedRoute,
    private toastr: ToastrService,
    private router: Router,
  ) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.posts = data['posts'].result;
      this.pagination = data['posts'].pagination;
    });
  }

  loadPosts(filter?: Filter) {
    let reset = false;
    let provider = this.route.snapshot.queryParams['provider'];
    let category = this.route.snapshot.queryParams['category'];
    // if(filter == undefined){
    //   reset = true;
    // }    
    if(filter){
      if(filter.provider != provider && !(filter.provider == '' && provider == undefined)){
        provider = filter.provider;
        reset = true;
      }
      if(filter.category != category && !(filter.category == '' && category == undefined)){
        category = filter.category;
        reset = true;
      }

      this.router.navigate([], {
        relativeTo: this.route,
        queryParams: {
          provider: provider,
          category: category
        },
        queryParamsHandling: 'merge',
        skipLocationChange: true
      });
    }
    if(reset || this.pagination.currentPage <= this.pagination.totalPages){
      this.postService
        .getPosts(
          reset ? 1 : this.pagination.currentPage,
          this.pagination.itemsPerPage,
          provider, category
          )
          .subscribe(
            (res: PaginatedResult<Post[]>) => {
              this.posts = reset ? res.result : this.posts.concat(res.result);
              this.pagination = res.pagination;
              reset = false;
            },
            error => {
              this.toastr.error(error);
            }
          );   
    }
  }
  
  pageChanged(): void {
    if(this.pagination.currentPage <= this.pagination.totalPages){
      this.pagination.currentPage = this.pagination.currentPage + 1;
    }
    this.loadPosts();
  }

  onScroll() {
    this.pageChanged()
  }

  UpdateOnDelete(){
    this.loadPosts();
  }

}

