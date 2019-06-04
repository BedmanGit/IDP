import { Component, OnInit, Input } from '@angular/core';
import { FormGroup, AbstractControl, FormControl, ValidationErrors } from '@angular/forms';
import { QuestionBase } from '../_models/QuestionBase';
import { ValidationError } from '../_models/ValidationError';
import { QuestionService } from '../_services/question.service';

@Component({
  selector: 'app-dynamic-form-question',
  templateUrl: './dynamic-form-question.component.html',
  styleUrls: ['./dynamic-form-question.component.css']
})
export class DynamicFormQuestionComponent implements OnInit {
  @Input() question: QuestionBase<any>;
  @Input() form: FormGroup;
  validErrors: string[];

  updateQuestions(key: string): void {
    let str = '';
    Object.keys(this.form.controls).forEach(e => {
      const _this = this.form;
      str = (str === '' ? '' : str + ',') + '"' + e + '":"' + _this.controls[e].value + '"' ;
    });
    const qa = JSON.parse('{' + str + '}');
    this.qs.updateHiddenQuestions(this.qs.getQuestions(), qa);
  }

  get isValid() { return this.form.controls[this.question.key].valid; }

  validate(ctrl: string): void {
    if (this.form.controls[ctrl]) {
      this.resetErrorMessageObj();
      if (this.form.controls[ctrl].errors) {
        this.validErrors = Object.keys(this.form.controls[ctrl].errors);
      }
    }
  }

  keys(obj: any) {
    if (obj !== null && obj !== undefined) {
      return Object.keys(obj);
    } else {
      return [];
    }
  }
  isEmpty(obj: any) {
    return obj.length === 0;
  }
  invalid(obj: any) {
    if (obj.length > 0) {
      return true;
    } else {
      return false;
    }
  }
  resetErrorMessageObj() {
    this.validErrors = null;
  }

  constructor(private qs: QuestionService) {
    this.resetErrorMessageObj();
  }

  ngOnInit() {
  }
}
