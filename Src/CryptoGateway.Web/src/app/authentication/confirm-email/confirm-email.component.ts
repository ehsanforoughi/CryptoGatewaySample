import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { AuthService } from '@core';
import { mergeMap } from 'rxjs';

@Component({
  selector: 'app-confirm-email',
  standalone: true,
  imports: [
    RouterLink
  ],
  templateUrl: './confirm-email.component.html',
  styleUrl: './confirm-email.component.scss'
})
export class ConfirmEmailComponent implements OnInit {
  confirmEmailMessage: string = '';
  constructor(private route: ActivatedRoute,
              private authService: AuthService
  ) { }
  
  ngOnInit(): void {
    this.route.queryParams.pipe(
      mergeMap((params) => this.authService.confirmEmail(params['email'], params['token'])),
    ).subscribe({
      next: (res) => {
        if (res) {
          this.confirmEmailMessage = 'Thank you for confirming your email';
        } else {
          this.confirmEmailMessage = 'Invalid Email Confirmation Request';
        }
      },
      error: (error) => {
        this.confirmEmailMessage = error;
      },
    });
  }
}
