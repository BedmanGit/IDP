import { Component, OnInit, OnDestroy } from '@angular/core';
import { QuestionBase } from '../_models/QuestionBase';
import { FormGroup, FormControl } from '@angular/forms';
import { QuestionControlService } from '../_services/question-control.service';
import { QuestionService } from '../_services/question.service';
import { Observable, Subject, BehaviorSubject, Subscription } from 'rxjs';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-dynamic-form',
  templateUrl: './dynamic-form.component.html',
  styleUrls: ['./dynamic-form.component.css']
})
export class DynamicFormComponent implements OnInit, OnDestroy {


  questions: QuestionBase<any>[] = [];
  form: FormGroup;
  subscription: Subscription;
  payLoad = '';

  constructor(private qs: QuestionService, private qcs: QuestionControlService) {
    // this.questions = qs.getQuestions();

    this.subscription = qs.getHiddenQuestions().subscribe(
      data => {
        this.questions = data;
      }, error => {
        console.log(error);
      }, () => {
        console.log('complete');
      }
    );
  }

  ngOnInit() {
    this.form = this.qcs.toFormGroup(this.questions);
  }

  onSubmit(): void {
    Object.keys(this.form.controls).forEach((ctrl: string) => {
      if (this.form.controls[ctrl].dirty) {
        this.form.controls[ctrl].updateValueAndValidity();
      }
    });
    this.payLoad = JSON.stringify(this.form.value);
  }


  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
