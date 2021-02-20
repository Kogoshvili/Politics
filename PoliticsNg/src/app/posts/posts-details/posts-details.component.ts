import { Component, OnInit } from '@angular/core';
import { Post } from 'src/app/_models/Post';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/_services/Auth.service';
import { PostService } from 'src/app/_services/Post.service';

@Component({
  selector: 'app-posts-details',
  templateUrl: './posts-details.component.html',
  styleUrls: ['./posts-details.component.css']
})
export class PostsDetailsComponent implements OnInit {
  post: Post;
  userId: number;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService, 
    private authService: AuthService, 
    private postService: PostService
  ) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.post = data['post'];
    });
    this.userId = this.authService.decodedToken.nameid;
  }

  deletePost(){
    this.postService.deletePost(this.userId, this.post.id).subscribe(
      next => {
        this.toastr.success('Success');
        this.router.navigate(['/home']);
      }, error => {
        this.toastr.error(error);
      }
    )
  }

}
