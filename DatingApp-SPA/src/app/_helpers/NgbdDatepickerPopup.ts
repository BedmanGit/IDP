import {Component, Input, Output, EventEmitter} from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';

@Component({
  selector: 'app-ngbd-datepicker-popup',
  template: `<div class="form-group" [formGroup]="formgp" >
  <input type="text" (change) = "updateQuestions(question.key)"
        placeholder="Datepicker" [formControlName]="key"
        class="form-control" name="key"
        bsDatepicker>
</div>`
})

export class NgbdDatepickerPopupComponent {
  @Input() key: string;
  @Input() formgp: FormGroup;

}
