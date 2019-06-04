import { QuestionBase } from './QuestionBase';

export class CheckboxQuestion extends QuestionBase<string> {
    controlType = 'checkbox';

    constructor(options: {} = {}) {
        super(options);
    }

}
