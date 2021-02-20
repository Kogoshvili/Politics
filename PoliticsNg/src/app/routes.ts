import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { PartiesComponent } from './parties/parties.component';
import { AuthGuard } from './_guards/auth.guard';
import { RegisterComponent } from './register/register.component';
import { PostsCreateComponent } from './posts/posts-create/posts-create.component';
import { PostEditResolver } from './_resolvers/post-edit.resolver';
import { PostsDetailsComponent } from './posts/posts-details/posts-details.component';
import { PostDetailResolver } from './_resolvers/post-detail.resolver';
import { PostListResolver } from './_resolvers/post-list.resolver';
import { UsersEditComponent } from './users/users-edit/users-edit.component';
import { UserEditResolver } from './_resolvers/user-edit.resolver';
import { ProviderDetailResolver } from './_resolvers/provider-details.resolver';
import { ResearchResolver } from './_resolvers/research.resolver';
import { RatingComponent } from './rating/rating.component';
import { RatingListResolver } from './_resolvers/rating-list.resolver';
import { ActivitySingleResolver } from './_resolvers/activity-single.resolver';
import { ActivitiesComponent } from './activities/activities.component';

export const mainRoutes: Routes = [
  { path: '',   redirectTo: 'home', pathMatch: 'full' },
  { path: 'home', component: HomeComponent, resolve: { posts: PostListResolver, candidates: ResearchResolver, activity: ActivitySingleResolver}},
  { path: 'parties/:provider', component: PartiesComponent, resolve: { posts: PostListResolver, provider: ProviderDetailResolver }},
  { path: 'register', component: RegisterComponent },
  { path: 'rating', component: RatingComponent, resolve: {topics: RatingListResolver}},
  { path: 'activities', component: ActivitiesComponent, resolve: {activity: ActivitySingleResolver}},
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      {
        path: 'post/create',
        component: PostsCreateComponent,
      },
      {
        path: 'post/edit/:id',
        component: PostsCreateComponent,
        resolve: { post: PostEditResolver }
      },
      // {
      //   path: 'user/edit',
      //   component: UsersEditComponent,
      //   resolve: { user: UserEditResolver }
      // }
    ]
  },
  { path: 'post/:id', component: PostsDetailsComponent, resolve: { post: PostDetailResolver }},
  { path: '**', redirectTo: '/register', pathMatch:  'full' },
];
//