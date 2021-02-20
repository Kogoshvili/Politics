import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/_models/User';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-users-edit',
  templateUrl: './users-edit.component.html',
  styleUrls: ['./users-edit.component.css']
})
export class UsersEditComponent implements OnInit {
  user: User;
  constructor(private route: ActivatedRoute, private toastr: ToastrService) { }

  ngOnInit() {
    this.route.data.subscribe(
      (data) => {
        this.user = data['user'];
      }
    )
  }

}
