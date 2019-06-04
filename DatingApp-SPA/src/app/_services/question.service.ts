import { Injectable } from '@angular/core';
import { QuestionBase } from '../_models/QuestionBase';
import { DropdownQuestion } from '../_models/DropdownQuestion';
import { TextboxQuestion } from '../_models/TextboxQuestion';
import { DatePicker } from '../_models/DatePicker';
import { BehaviorSubject, Subject, Observable } from 'rxjs';
import { CheckboxQuestion } from '../_models/CheckboxQuestion';

@Injectable()
export class QuestionService {

  private hiddenSubject;
  // TODO: get from a remote source of question metadata
  // TODO: make asynchronous
  getQuestions() {

    const questions: QuestionBase<any>[] = [
      new DropdownQuestion({
        questionId: 1,
        key: 'site',
        label: 'Site Number',
        options: [
          { key: '001', value: 'Brooklyn' },
          { key: '002', value: 'Queens' },
          { key: '003', value: 'Staten Island' },
          { key: '004', value: 'Browns' }
        ],
        value: '001',
        // tslint:disable-next-line:max-line-length
        customValidations: '{"requiredQuestions":[{"site":"001","firstName":"James"}], "requiredQuestionsMsg":"Required questions: site = 001, firstName = James"}',
        order: 1
      }),
      new TextboxQuestion({
        questionId: 2,
        key: 'firstName',
        label: 'First name',
        value: 'James',
        htmlValidations: {
          required: true,
          minLength: 4,
          maxLength: 6
        },
        // tslint:disable-next-line:max-line-length
        customValidations: '{"requiredQuestions":[{"site":"001","firstName":"James"}], "requiredQuestionsMsg":"Required questions: site = 001, firstName = James"}',
        order: 2
      }),
      new TextboxQuestion({
        questionId: 3,
        key: 'lastName',
        label: 'Last name',
        value: 'Chen',
        //  required: true,
        order: 3,
        htmlValidations: {
          required: true,
          minLength: 4,
          maxLength: 6
        },
        // tslint:disable-next-line:max-line-length
        customValidations: '{"requiredQuestions":[{"site":"001","firstName":"James"}], "requiredQuestionsMsg":"Required questions: site = 001, firstName = James"}',
      }),
      new TextboxQuestion({
        questionId: 4,
        key: 'emailAddress',
        label: 'Email',
        type: 'email',
        htmlValidations: {
          required: true,
          email: true
        },
        order: 4
      }),
      new DatePicker({
        questionId: 5,
        key: 'dob',
        label: 'Birthday',
        htmlValidations: {
          required: true
        },
        order: 5
      }),
      new CheckboxQuestion({
        questionId: 6,
        key: 'shelter',
        label: 'Shelter',
        htmlValidations: {
          required: true
        },
        show: false,
        showIf: [{ site: '001', firstName: 'James' }, { site: '002' }],
        value: false,
        order: 6
      })
    ];

    return questions.sort((a, b) => a.order - b.order);
  }

  getHiddenQuestions(): BehaviorSubject<any> {
    /* const hideArray = [];
     this.getQuestions().forEach(q => {
       if (q.showIf !== null && q.showIf !== undefined) {
         if (q.showIf.length > 0) {
           hideArray.push(q.key);
         }
       }
     });*/
    this.hiddenSubject = new BehaviorSubject<any>(this.getQuestions());
    return this.hiddenSubject;
  }

  updateHiddenQuestions(questions: QuestionBase<any>[], answers: any) {
    let tof = false;
    questions.forEach(question => {
      tof = false;
      if (question.showIf !== undefined && question.showIf !== null) {
        question.showIf.forEach(obj => {  // showif json object is OR
          tof = tof || false;
          for (const key in obj) {  // showif properties is AND
            if (answers[key] !== undefined) {
              if (answers[key] === obj[key]) {
                tof = true;
              } else {
                tof = tof || false;
                break;  // not meet
              }
            } else {
              console.log('Question not exists: ' + key);
            }
          }
        });
        tof ? question.show = true :  question.show = false;
      }
    });
    this.hiddenSubject.next(questions);
  }

}
