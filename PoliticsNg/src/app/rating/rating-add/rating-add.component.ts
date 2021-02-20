import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { RatingService } from 'src/app/_services/Rating.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-rating-add',
  templateUrl: './rating-add.component.html',
  styleUrls: ['./rating-add.component.css']
})
export class RatingAddComponent implements OnInit {
  addModelRef: BsModalRef;
  model: any = {};

  constructor(
    private ratingService: RatingService,
    private modalService: BsModalService,
    private toastr: ToastrService,
  ) { }

  ngOnInit() {
  }

  
  openModal(template: TemplateRef<any>) {
    this.addModelRef = this.modalService.show(template, { class: 'modal-lg' });
  }

  sendTopic(){
    this.ratingService.offerTopic(this.model).subscribe(
      ()=>{
        this.toastr.success("Success");
        this.addModelRef.hide();
      }, error => {
        this.toastr.error(error);
      }
    )
  }

}
