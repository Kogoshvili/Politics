import { Component, OnInit, Input, Output, EventEmitter, TemplateRef } from '@angular/core';
import { Post } from 'src/app/_models/Post';
import { DatePipe } from '@angular/common';
import { QuillDeltaToHtmlConverter } from 'quill-delta-to-html';
import {NgxGalleryOptions} from '@kolkov/ngx-gallery';
import {NgxGalleryImage} from '@kolkov/ngx-gallery';
import {NgxGalleryAnimation} from '@kolkov/ngx-gallery';
import { AuthService } from 'src/app/_services/Auth.service';
import { PostService } from 'src/app/_services/Post.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';


@Component({
  selector: 'app-posts-card',
  templateUrl: './posts-card.component.html',
  styleUrls: ['./posts-card.component.css']
})
export class PostsCardComponent implements OnInit {
  @Input() post: Post;
  @Output() UpdateOnDelete = new EventEmitter();
  hide: boolean = true;
  converter: any;
  html: any;
  userId: any = null;
  url: string;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];
  modalRef4: BsModalRef;
  like: boolean;

  constructor(
    private authService: AuthService,
    private postService: PostService,
    private toastr: ToastrService,
    private modalService: BsModalService,
    private router: Router
  ) { }

  ngOnInit() {
    this.converter = new QuillDeltaToHtmlConverter(JSON.parse(this.post.content.replace(/\\/g, '')).ops, {});
    this.post.content = this.converter.convert();

    this.url = "http://localhost:4200/post/" + this.post.id;
    if(this.authService.loggedIn()){
      this.userId = this.authService.decodedToken.nameid;
      if(this.post.likes.includes(parseInt(this.userId))){
        this.like = true;
      }
    }

    this.galleryOptions = [
      {
        imageSize: "contain",
        imageArrowsAutoHide: this.post.images.length > 1 ? true : false,
        imageArrows: this.post.images.length > 1,
        thumbnailsRemainingCount: true,
        thumbnailsArrowsAutoHide: true,
        thumbnails: this.post.images.length > 1,
        previewArrows: this.post.images.length > 1,
        width: '600px',
        height: '400px',
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide
      },
      // max-width 800
      {
        thumbnailsColumns: 6,
        breakpoint: 800,
        width: '100%',
        height: '600px',
        imagePercent: 80,
        thumbnailsPercent: 20,
        // thumbnailsMargin: 20,
        // thumbnailMargin: 20
      },
      // max-width 400
      {
        breakpoint: 430,
        thumbnailsColumns: 4,
        // preview: false
      },
      {
        breakpoint: 325,
        thumbnailsColumns: 3,
      }
    ];
    this.galleryImages = []
    this.post.images.forEach(
      (img) => {
        this.galleryImages.push({
          small: img,
          medium: img,
          big: img
          })
      }
    )
  }

  showhide(){
    this.hide = !this.hide;
  }

  checkOverflow (element: any) {
    if (element.offsetHeight < element.scrollHeight) {
      return true;
    } else {
      return false;
    }
  }

  editPost(){
    if(this.userId == this.post.user.id){
      this.router.navigate(['post/edit/' + this.post.id]);
    }else{
      this.toastr.error('Unauthorised')
    }
  }

  deletePost(){
    this.postService.deletePost(this.userId, this.post.id).subscribe(
      next => {
        this.toastr.success('Success');
        this.modalRef4.hide();
        //this.UpdateOnDelete.emit();
        window.location.reload();
      }, error => {
        this.toastr.error(error);
      }
    )
  }

  openDelete(template: TemplateRef<any>){
    this.modalRef4 = this.modalService.show(template, { class: 'modal-sm' });
  }

  likePost(){
    if(this.userId != null){
      this.postService.likePost(this.userId, this.post.id).subscribe(
        next => {
          if(this.like){
            this.post.likes = this.post.likes.filter(obj => obj !== parseInt(this.userId));
          }else{
            this.post.likes.push(parseInt(this.userId));
          }
          this.like = !this.like;
        },
        error => {
          this.toastr.error(error);
        }
      )
    }
  }
}
