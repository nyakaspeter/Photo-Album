import { Component, OnInit } from '@angular/core';
import { AccountClient, GroupClient, GroupDto, UserDto } from 'src/app/api/app.generated';
import { SnackbarService } from '../snackbar/snackbar.service';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss']
})
export class UsersComponent implements OnInit {
  public group: GroupDto;
  originalUserIds: number[] = [];
  users: UserDto[] = [];
  selectedUserIds: number[] = [];

  constructor(
    private accountClient: AccountClient,
    private groupClient: GroupClient,
    private snackbarService: SnackbarService
  ) { }

  ngOnInit(): void {
    this.accountClient.getUsers().subscribe(
      (r) => {
        this.users = r;
      },
      (error) => {
        this.snackbarService.openError(error.detail);
      }
    );

    this.originalUserIds = this.group.users.map(user => user.id);

    this.group.users.forEach(user => {
      this.selectedUserIds.push(user.id);
    });
  }

  save(): void {
    this.originalUserIds.forEach(originalUserId => {
      if (!this.selectedUserIds.includes(originalUserId)) {
        this.groupClient.deleteUserFromGroup(originalUserId, this.group.id).subscribe(
          () => {
            this.snackbarService.openSuccess("Group members update successful");
          },
          (error) => {
            this.snackbarService.openError(error.detail);
          }
        );
      }
    })

    this.selectedUserIds.forEach(newUserId => {
      if (!this.originalUserIds.includes(newUserId)) {
        this.groupClient.addUserToGroup(newUserId, this.group.id).subscribe(
          () => {
            this.snackbarService.openSuccess("Group members update successful");
          },
          (error) => {
            this.snackbarService.openError(error.detail);
          }
        );
      }
    });
  }
}
