import { NgIf } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ILogin } from '../../models/master/login';
import { Router } from '@angular/router';
import { MasterService } from '../../services/master/master.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [NgIf, ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  loginForm: FormGroup;
  masterSrc = inject(MasterService);
  router = inject(Router);
  constructor(private fb: FormBuilder) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }
  onSubmit() {
    if (this.loginForm.valid) {
      const data: ILogin = this.loginForm.value;
      // console.log('Login form data:', this.loginForm.value);
      this.masterSrc.logIn(data).subscribe({
        next: (res: any) => {
          alert(res.message);
          if (res.statusCode === 200) {
            localStorage.setItem("userObj", JSON.stringify(res.user));
            // this.masterSrc.userObj = res.user;
            this.masterSrc.setUser(res.user);
            this.router.navigateByUrl("/todo");
          }
        },
        error: (error) => {
          alert(error)
        }
      })
    }
  }
}

