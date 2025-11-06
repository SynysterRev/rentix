import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from "@angular/common";
import { passwordMatchValidator } from '../../validators/password-match.validator';

@Component({
  selector: 'app-settings',
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './settings.html',
  styleUrl: './settings.scss',
})
export class Settings {

  // Define the FormGroup with controls and validators
  userForm = new FormGroup({
    name: new FormControl('aaa', [Validators.required, Validators.minLength(3)]),
    firstname: new FormControl('aaa', [Validators.required, Validators.minLength(3)]),
    email: new FormControl('a.a@a.a', [Validators.required, Validators.email]),
    // password: new FormControl('', [Validators.pattern('^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$')]),
    phonenumber: new FormControl('0650390745', [Validators.required, Validators.pattern(/^0\d{9}$/)])
  });

  passwordForm = new FormGroup({
    actualpassword: new FormControl('', [Validators.required]),
    newpassword: new FormControl('', [Validators.required, Validators.pattern(/(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@$!%*#?&^_-]).{8,}/)]),
    confirmnewpassword: new FormControl('', [Validators.required, Validators.pattern(/(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@$!%*#?&^_-]).{8,}/)]),

  }, { validators: passwordMatchValidator() });

  onSubmitUserForm() {
    if (this.userForm.valid) {
      console.log('Form submitted:', this.userForm.value);
    } else {
      console.log('Form is invalid');
    }
  }

  onSubmitPasswordForm() {
    if (this.passwordForm.valid) {
      console.log('Form submitted:', this.passwordForm.value);
    } else {
      console.log('Form is invalid');
    }
  }
}
