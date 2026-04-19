import { NgIf } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { IRegister } from '../../models/master/register';
import { MasterService } from '../../services/master/master.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [NgIf, ReactiveFormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  registerForm: FormGroup;
  masterSrc = inject(MasterService);
  router = inject(Router);
  constructor(private fb: FormBuilder) {
    // this.registerForm = this.fb.group<IRegister>({
    //   userName: ['', Validators.required],
    //   password: ['', [Validators.required, Validators.minLength(6)]],
    //   email: ['', [Validators.required, Validators.email]],
    //   phone: ['', [Validators.required, Validators.pattern(/^\d{10}$/)]],
    //   city: ['', Validators.required]
    // });

    // this.registerForm = this.fb.group({
    //   userName: ['', Validators.required],
    //   password: ['', [Validators.required, Validators.minLength(6)]],
    //   email: ['', [Validators.required, Validators.email]],
    //   phone: ['', [Validators.required, Validators.pattern(/^\d{10}$/)]],
    //   city: ['', Validators.required]
    // });

    this.registerForm = new FormGroup({
      userName: new FormControl<string>('', Validators.required),
      password: new FormControl<string>('', [Validators.required, Validators.minLength(6)]),
      email: new FormControl<string>('', [Validators.required, Validators.email]),
      phone: new FormControl<string>('', [Validators.required, Validators.pattern(/^\d{10}$/)]),
      city: new FormControl<string>('', Validators.required)
    });
  }

  onSubmit() {
    if (this.registerForm.valid) {
      const data: IRegister = this.registerForm.value;
      // console.log('Register Data:', data);
      this.masterSrc.register(data).subscribe({
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
