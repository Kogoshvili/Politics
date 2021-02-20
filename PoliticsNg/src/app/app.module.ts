import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { NavbarComponent } from './navbar/navbar.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from './_services/Auth.service';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { ErrorInterceptorProvider } from './_services/error.interceptor';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BsDropdownModule} from 'ngx-bootstrap/dropdown';
import { TabsModule} from 'ngx-bootstrap/tabs';
import { ButtonsModule} from 'ngx-bootstrap/buttons';
import { PaginationModule} from 'ngx-bootstrap/pagination';
import { ModalModule } from 'ngx-bootstrap/modal';
import { PartiesComponent } from './parties/parties.component';
import { RouterModule } from '@angular/router';
import { mainRoutes } from './routes';
import { PostDetailResolver } from './_resolvers/post-detail.resolver';
import { PostEditResolver } from './_resolvers/post-edit.resolver';
import { PostListResolver } from './_resolvers/post-list.resolver';
import { PostsCreateComponent } from './posts/posts-create/posts-create.component';
import { PostsListComponent } from './posts/posts-list/posts-list.component';
import { PostsDetailsComponent } from './posts/posts-details/posts-details.component';
import { JwtModule } from '@auth0/angular-jwt';
import { PostsCardComponent } from './posts/posts-card/posts-card.component';
import { QuillModule } from 'ngx-quill';
import { PostsFilterComponent } from './posts/posts-filter/posts-filter.component';
import { UsersEditComponent } from './users/users-edit/users-edit.component';
import { UserEditResolver } from './_resolvers/user-edit.resolver';
import { UserService } from './_services/User.service';
import { ImageUploadModule } from "angular2-image-upload";
import { NgxGalleryModule } from '@kolkov/ngx-gallery';
import { ProviderDetailResolver } from './_resolvers/provider-details.resolver';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { ResearchComponent } from './research/research.component';
import { ElectionService } from './_services/Election.service';
import { ResearchResolver } from './_resolvers/research.resolver';
import { NgxSpinnerModule } from "ngx-spinner";
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { RatingComponent } from './rating/rating.component';
import { RatingListComponent } from './rating/rating-list/rating-list.component';
import { RatingCardComponent } from './rating/rating-card/rating-card.component';
import { RatingListResolver } from './_resolvers/rating-list.resolver';
import { RatingCardCommentComponent } from './rating/rating-card/rating-card-comment/rating-card-comment.component';
import { RatingCardCommmentsListComponent } from './rating/rating-card/rating-card-commments-list/rating-card-commments-list.component';
import { RatingCommentCreateComponent } from './rating/rating-card/rating-comment-create/rating-comment-create.component';
import { RatingAddComponent } from './rating/rating-add/rating-add.component';
import { ToastrModule } from 'ngx-toastr';
import { ActivitiesComponent } from './activities/activities.component';
import { ActivityService } from './_services/Activity.service';
import { ActivitySingleResolver } from './_resolvers/activity-single.resolver';
import { ActivitiesCommentsComponent } from './activities/activities-comments/activities-comments.component';
import { ActivitiesCreateCommentComponent } from './activities/activities-create-comment/activities-create-comment.component';

export function tokenGetter() {
   return localStorage.getItem('token');
}

@NgModule({
   declarations: [
      AppComponent,
      NavbarComponent,
      HomeComponent,
      RegisterComponent,
      PartiesComponent,
      PostsCreateComponent,
      PostsListComponent,
      PostsDetailsComponent,
      PostsCardComponent,
      PostsFilterComponent,
      UsersEditComponent,
      ResearchComponent,
      RatingComponent,
      RatingListComponent,
      RatingCardComponent,
      RatingCardCommentComponent,
      RatingCardCommmentsListComponent,
      RatingCommentCreateComponent,
      RatingAddComponent,
      ActivitiesComponent,
      ActivitiesCommentsComponent,
      ActivitiesCreateCommentComponent
   ],
   imports: [
      BrowserModule,
      HttpClientModule,
      FormsModule,
      ReactiveFormsModule,
      BrowserAnimationsModule,
      BsDropdownModule.forRoot(),
      ButtonsModule.forRoot(),
      PaginationModule.forRoot(),
      TabsModule.forRoot(),
      ModalModule.forRoot(),
      RouterModule.forRoot(mainRoutes),
      QuillModule.forRoot({
         modules:{
            toolbar: [
               ['bold', 'italic', 'underline', 'strike'],        // toggled buttons
               ['blockquote'],
            
               [{ 'list': 'ordered'}, { 'list': 'bullet' }],
               [{ 'script': 'sub'}, { 'script': 'super' }],      // superscript/subscript
               [{ 'indent': '-1'}, { 'indent': '+1' }],          // outdent/indent
               [{ 'direction': 'rtl' }],                         // text direction
            
               [{ 'size': ['small', false, 'large'] }],
            
               [{ 'color': [] }, { 'background': [] }],          // dropdown with defaults from theme
               [{ 'font': [] }],
               [{ 'align': [] }],
            
               ['clean'],                                         // remove formatting button
            
               ['link']                         // link and image, video
             ],
         }
      }),
      JwtModule.forRoot({
         config: {
            tokenGetter,
            allowedDomains: ['localhost:5001'],
            disallowedRoutes: ['localhost:5001/api/auth']
         }
       }),
      ImageUploadModule.forRoot(),
      NgxGalleryModule,
      InfiniteScrollModule,
      NgxSpinnerModule,
      CollapseModule.forRoot(),
      ToastrModule.forRoot()
   ],
   exports: [
      QuillModule
   ],
   providers: [
      ErrorInterceptorProvider,
      AuthService,
      PostDetailResolver,
      PostEditResolver,
      PostListResolver,
      UserEditResolver,
      UserService,
      ProviderDetailResolver,
      ElectionService,
      ResearchResolver,
      RatingListResolver,
      ActivityService,
      ActivitySingleResolver
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
platformBrowserDynamic().bootstrapModule(AppModule);
