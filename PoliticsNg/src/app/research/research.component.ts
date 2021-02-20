import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Candidate } from '../_models/Candidate';
import { ElectionService } from '../_services/Election.service';
import { Vote } from '../_models/Vote';
import { AuthService } from '../_services/Auth.service';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-research',
  templateUrl: './research.component.html',
  styleUrls: ['./research.component.css']
})
export class ResearchComponent implements OnInit {
  researchVote: FormGroup;
  candidates: Candidate[];
  voterExists: boolean;
  
  constructor(
    public fb: FormBuilder,
    private route: ActivatedRoute,
    private toastr: ToastrService,
    private authService: AuthService,
    private router: Router,
    private electionService: ElectionService,
    private spinner: NgxSpinnerService
  ) { }

  ngOnInit() {
    this.spinner.hide();
    this.researchVote = this.fb.group({
      prime: [null, Validators.required],
      first: [null],
      second: [null],
      third: [null],
      fourth: [null],
      fifth: [null]
    }, {validator: this.onlyOnce})
    
    if(this.authService.loggedIn()){
      
      this.electionService.voterExists(this.authService.decodedToken.nameid, this.authService.decodedToken.unique_name).subscribe(
        next => {
          this.voterExists = next;
        }, error => {
          this.voterExists = true;
        }
      );

    }else{
      this.voterExists = false;
    }

    this.route.data.subscribe(data => {
      this.candidates = data['candidates']
    });
  }

  onlyOnce(g: FormGroup) {
    let list = [g.get('first').value, g.get('second').value, g.get('third').value, g.get('fourth').value, g.get('fifth').value]
    list = list.filter(function(element) {return element !== null; }); 
    var counts = {};
    for (var i = 0; i < list.length; i++) {
      var num = list[i];
      counts[num] = counts[num] ? counts[num] + 1 : 1;
    }
    if( counts[g.get('first').value] > 1 || counts[g.get('second').value] > 1 || 
        counts[g.get('third').value] > 1 || counts[g.get('fourth').value] > 1 || 
        counts[g.get('fifth').value] > 1){
          return {'mismatch': true};
        }
    return null
  }

  changeCity(e) {
    this.researchVote.setValue(e.target.value, {
      onlySelf: true
    })
  }

  vote(){
    if(!this.authService.loggedIn()){
      this.toastr.error("Please Login")
    }else{
      let vote = this.researchVote.value as Vote;
      this.electionService.registerVote(this.authService.decodedToken.nameid, vote).subscribe(
        next => {
          this.toastr.success('Vote registered');
          this.voterExists = true;
        }, error => {
          this.toastr.error(error);
        }
      )
    }
  }

}
