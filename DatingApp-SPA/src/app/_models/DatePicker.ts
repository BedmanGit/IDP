

import { QuestionBase } from './QuestionBase';

export class DatePicker extends QuestionBase<string> {
    controlType = 'datePicker';
    type: string;

    constructor(options: {} = {}) {
        super(options);
        this.type = options['type'] || '';
    }
}
