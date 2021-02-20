import { Component, OnInit, TemplateRef, HostListener } from '@angular/core';
import { AuthService } from '../_services/Auth.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { combineAll } from 'rxjs/operators';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { UserRegister } from '../_models/UserRegister';
import { PostService } from '../_services/Post.service';
import { Provider } from '../_models/Provider';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  modalRef2: BsModalRef;
  modalRef3: BsModalRef;
  modalRef: BsModalRef | null;
  user: UserRegister;
  providers: Provider[];
  tokenData: any;
  isCollapsed: boolean = true;
  sidenav: boolean;
  parties: boolean;
  loggedIn: boolean;
  innerWidth: any;

  constructor(
    private authService: AuthService, 
    private toastr: ToastrService, 
    private router: Router, 
    private modalService: BsModalService,
    private postService: PostService,
    private spinner: NgxSpinnerService
  ) { }

  ngOnInit() {
    if(innerWidth > 1700){
      this.sidenav = true;
    }
    this.user = {} as UserRegister;
    this.tokenData = this.authService.decodedToken;
    this.postService.getProviderss().subscribe(
      (res) => {
        this.providers = res;
      },error => {
        this.toastr.error(error);
      }
    )
    this.loggedIn = this.authService.loggedIn() || false;
  }

  login() {
    this.authService.login(this.user).subscribe(next => {
      this.toastr.success('Logged in successfully!');
      this.modalRef3.hide();
      this.loggedIn = this.authService.loggedIn() || false;
      window.location.reload();
    }, error => {
      this.toastr.error(error);
    });
  }

  logout(){
    localStorage.removeItem('token');
    this.toastr.success('Logged out in successfully!');
    this.loggedIn = this.authService.loggedIn() || false;
    window.location.reload()
  }

  openRegister(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
  }

  openLogin(template: TemplateRef<any>) {
    this.modalRef3 = this.modalService.show(template, { class: 'modal-sm' });
  }
  
  openCode(template: TemplateRef<any>) {
    this.authService.sendSMS(this.user).subscribe(
      res => {
        this.toastr.success('Success');
        this.modalRef2 = this.modalService.show(template, { class: 'second' });
      }, error => {
        this.toastr.error(error);
      }
    )
  }

  register() {
    this.authService.register(this.user).subscribe(() => {
        this.toastr.success('Registration successful');
        this.modalRef2.hide();
        this.closeFirstModal();
        window.location.reload();
      }, error => {
        this.toastr.error(error);
      });
  }

  closeFirstModal() {
    if (!this.modalRef) {
      return;
    }
 
    this.modalRef.hide();
    this.modalRef = null;
  }

  loading(url?: string){
    if(this.router.url !== url){
      this.spinner.show();
    }
    if(innerWidth < 990){
      this.toggleNav();
    }
  }

  toggleNav(){
    this.sidenav = !this.sidenav;
  }
  toggleParties(){
    this.parties = !this.parties;
  }
  
}
