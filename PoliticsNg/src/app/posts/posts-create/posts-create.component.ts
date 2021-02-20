import { Component, OnInit, ViewChild } from '@angular/core';
import { PostFull } from 'src/app/_models/PostFull';
import { Image } from 'src/app/_models/Image';
import { NgForm, FormGroup, FormBuilder, FormArray, FormControl, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/_services/Auth.service';
import { PostService } from 'src/app/_services/Post.service';
import { Category } from 'src/app/_models/Category';
import { JsonPipe } from '@angular/common';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-posts-create',
  templateUrl: './posts-create.component.html',
  styleUrls: ['./posts-create.component.css']
})
export class PostsCreateComponent implements OnInit {
  post: PostFull;
  Images = [];
  PastImages = [];
  postCreate: FormGroup;
  categories: Category[];
  customStyle = {
    selectButton: {
      "background-color": "#fed766",
      "border-radius": "10px",
      "color": "#000"
    },
    clearButton: {
      "background-color": "#fed766",
      "border-radius": "25px",
      "color": "#000",
      "margin-left": "10px"
    },
    layout: {
      "background-color": "#02B875",
      "border-radius": "10px",
      "color": "#000",
      "font-size": "15px",
      "margin": "10px",
      "padding-top": "5px",
    },
    previewPanel: {
      "background-color": "#02B875",
      "border-radius": "0 0 25px 25px",
    }
  }
  
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService, 
    private authService: AuthService, 
    private postService: PostService,
    public fb: FormBuilder,
    private spinner: NgxSpinnerService
  ) { 
    this.postCreate = this.fb.group({
      category: [''],
      content: [''],
      Images: [null]
    })
  }

  ngOnInit() {
    this.post = {} as PostFull;
    this.route.data.subscribe(data => {
      if(data['post'] != undefined){
        this.post = data['post'];
        this.post.images.forEach(
          (img)=>{
            this.PastImages.push(
              {
                id: img.id,
                url: img.imageUrl,
                publicId: img.publicId
              }
            );
          }
        )
        this.postCreate.patchValue({
          category: this.post.category['name'],
          content: this.post.content,
        })
      }
    },error=>{},()=>{this.spinner.hide();});

    this.postService.getCategories().subscribe(
      (res) => {
        this.categories = res;
      }, error => {
        this.toastr.error(error);
      }
    )
  }

 

  uploadFile(event) {
    this.Images.push(event.file);
    
    this.postCreate.patchValue({
      Images: this.Images
    });
    this.postCreate.get('Images').updateValueAndValidity()
  }

  onRemoved(event){
    let deleted = false;
    this.PastImages.forEach(
      (img,i) => {
        if(img.url == event.src){
          this.PastImages.splice(i,1);
          deleted = true;
        }
      }
    )
    if(!deleted){
      this.Images.forEach(
        (img, i) => {
          if(img == event.file){
            this.Images.splice(i, 1);
          }
        }
      )
      this.postCreate.patchValue({
        Images: this.Images
      });
  
      this.postCreate.get('Images').updateValueAndValidity();
    }
  }

  createPost() {
    var formData: any = new FormData();
    formData.append("content", this.postCreate.get('content').value);
    this.Images.forEach(
        (img) => {
          formData.append("Images", img);
        }
      )
      
    this.categories.forEach(
      (c)=>{
        if(c.name == this.postCreate.get('category').value){
          formData.append("category", JSON.stringify(c));
        }
      }
    )
    
    if(this.post.id){
      formData.append("id", this.post.id);

      if(this.post.images.length > 0){
        let imgs = this.post.images.filter(e => this.PastImages.find(a => e.id == a.id));
        imgs.forEach(
          (i) => {
            formData.append("ImagesToSave", JSON.stringify({
              id: i.id,
              imageUrl: i.imageUrl,
              publicId: i.publicId
            }));
          }
        )
      }
      // for (var pair of formData.entries()) {
      //     console.log(pair[0]+ ', ' + pair[1]); 
      // }
        
      this.postService
        .updatePost(this.authService.decodedToken.nameid, formData)
        .subscribe(
          next => {
            this.toastr.success('Post updated successfully');
            this.router.navigate(['home'])
          },
          error => {
            this.toastr.error(error);
          }
        );

    }else{

      this.postService
        .createPost(this.authService.decodedToken.nameid, formData)
        .subscribe(
          next => {
            this.toastr.success('Post created successfully');
            this.router.navigate(['home'])
          },
          error => {
            this.toastr.error(error);
          }
        );

    }
  }
}
