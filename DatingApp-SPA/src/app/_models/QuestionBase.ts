import { AbstractControl, ValidatorFn } from '@angular/forms';

export class QuestionBase<T> {
    questionId: number;
    value: T;
    key: string;
    label: string;
    order: number;
    controlType: string;
    htmlValidations: {
      min: number,
      max: number,
      required: boolean,
      requiredTrue: boolean,
      email: boolean,
      minLength: number,
      maxLength: number,
      pattern: RegExp,
      nullValidator: boolean,
      compose: ValidatorFn[],
      composeAsync: ValidatorFn[]
    };
    customValidations: string;
    showIf: Array<T>;
    show: boolean;
    constructor(options: {
        questionId?: number;
        value?: T,
        key?: string,
        label?: string,
        required?: boolean,
        maxLength?: number,
        order?: number,
        htmlValidations?: {
          min: number,
          max: number,
          required: boolean,
          requiredTrue: boolean,
          email: boolean,
          minLength: number,
          maxLength: number,
          pattern: RegExp,
          nullValidator: boolean,
          compose: ValidatorFn[],
          composeAsync: ValidatorFn[]
        },
        controlType?: string,
        customValidations?: string,
        showIf?: Array<T>,
        show?: boolean
      } = {}) {
      this.questionId = options.questionId === undefined ? options.order : options.questionId;
      this.value = options.value;
      this.key = options.key || '';
      this.label = options.label || '';
      this.order = options.order === undefined ? 1 : options.order;
      this.controlType = options.controlType || '';
      this.htmlValidations = options.htmlValidations;
      this.customValidations = options.customValidations || '';
      this.showIf = options.showIf;
      this.show = options.show;
    }
}
