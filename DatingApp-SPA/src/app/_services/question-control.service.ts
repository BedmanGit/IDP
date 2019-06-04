import { Injectable } from '@angular/core';
import { QuestionBase } from '../_models/QuestionBase';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { customerValidations, customerValidations2 } from '../_helpers/custom-validation';
import { forEach } from '@angular/router/src/utils/collection';

@Injectable({
  providedIn: 'root'
})
export class QuestionControlService {

  constructor() { }

  toFormGroup(questions: QuestionBase<any>[]) {
    const group: any = {};
    questions.forEach(question => {
      const validatorFns = [];
      // tslint:disable-next-line:max-line-length
      group[question.key] = new FormControl(question.value || '');

     /* if (question.customValidations !== null && question.customValidations !== '') {
        validatorFns.push(customerValidations2(question.customValidations));
      }*/

      // tslint:disable-next-line: forin
      for (const obj in question.htmlValidations) {
        switch (obj) {
          case 'maxLength': {
            validatorFns.push(Validators.maxLength(question.htmlValidations.maxLength));
            break;
          }
          case 'minLength': {
            validatorFns.push(Validators.minLength(question.htmlValidations.minLength));
            break;
          }
          case 'required': {
            validatorFns.push(Validators.required);
            break;
          }
          case 'email': {
            validatorFns.push(Validators.email);
            break;
          }
          default: {
            console.log('hit default');
            break;
          }
        }
      }
      group[question.key].setValidators(Validators.compose(validatorFns));
    });
    // return new FormGroup(group, customerValidations(questions));
    return new FormGroup(group, customerValidations(questions));
  }
}
