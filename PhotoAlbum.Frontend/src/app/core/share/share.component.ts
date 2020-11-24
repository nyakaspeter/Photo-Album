import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AccountClient, AlbumClient, AlbumDto, GroupClient, GroupDto, UserDto } from 'src/app/api/app.generated';
import { SnackbarService } from '../snackbar/snackbar.service';
import { FormControl } from '@angular/forms';
import { MatAutocomplete, MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';

@Component({
  selector: 'app-share',
  templateUrl: './share.component.html',
  styleUrls: ['./share.component.scss'],
})
export class ShareComponent implements OnInit {
  groupCtrl = new FormControl();
  userCtrl = new FormControl();

  public album: AlbumDto;

  groups: GroupDto[] = [];
  originalGroupIds: number[] = [];
  allGroups: GroupDto[] = [];
  filteredGroups: Observable<GroupDto[]>;

  users: UserDto[] = [];
  originalUserIds: number[] = [];
  allUsers: UserDto[] = [];
  filteredUsers: Observable<UserDto[]>;

  @ViewChild('groupInput') groupInput: ElementRef<HTMLInputElement>;
  @ViewChild('userInput') userInput: ElementRef<HTMLInputElement>;
  @ViewChild('autoGroup') matAutocompleteGroup: MatAutocomplete;
  @ViewChild('autoUser') matAutocompleteUser: MatAutocomplete;

  constructor(
    private albumClient: AlbumClient,
    private groupClient: GroupClient,
    private accountClient: AccountClient,
    private snackbarService: SnackbarService,
    public dialog: MatDialog
  ) { }

  ngOnInit(): void {
    this.groups = this.album.groupsWithAccess;
    this.users = this.album.usersWithAccess;

    this.originalGroupIds = this.groups.map(group => group.id);
    this.originalUserIds = this.users.map(user => user.id);

    this.groupClient.getGroups().subscribe(
      (r) => {
        this.allGroups = r;
        this.filteredGroups = this.groupCtrl.valueChanges.pipe(
          startWith(null),
          map((groupName: string | null) => groupName ? this._filterGroups(groupName) : this.allGroups.slice()));
      },
      (error) => {
        this.snackbarService.openError(error.detail);
      }
    );

    this.accountClient.getUsers().subscribe(
      (r) => {
        this.allUsers = r;
        this.filteredUsers = this.userCtrl.valueChanges.pipe(
          startWith(null),
          map((userName: string | null) => userName ? this._filterUsers(userName) : this.allUsers.slice()));
      },
      (error) => {
        this.snackbarService.openError(error.detail);
      }
    );
  }

  removeGroup(group: GroupDto): void {
    const index = this.groups.indexOf(group);

    if (index >= 0) {
      this.groups.splice(index, 1);
    }
  }

  removeUser(user: UserDto): void {
    const index = this.users.indexOf(user);

    if (index >= 0) {
      this.users.splice(index, 1);
    }
  }

  selectedGroup(event: MatAutocompleteSelectedEvent): void {
    var groupToAdd: GroupDto;
    var groupName = event.option.viewValue;

    let foundGroups = this.allGroups.filter(group => group.name == groupName);
    if (foundGroups.length) {
      groupToAdd = foundGroups[0];
      if (!this.groups.some(group => group.id === groupToAdd.id)) {
        this.groups.push(groupToAdd);
      }
    }

    this.groupInput.nativeElement.value = '';
    this.groupCtrl.setValue(null);
  }

  selectedUser(event: MatAutocompleteSelectedEvent): void {
    var userToAdd: UserDto;
    var userName = event.option.viewValue;

    let foundUsers = this.allUsers.filter(user => user.userName == userName);
    if (foundUsers.length) {
      userToAdd = foundUsers[0];
      if (!this.users.some(user => user.id === userToAdd.id)) {
        this.users.push(userToAdd);
      }
    }

    this.userInput.nativeElement.value = '';
    this.userCtrl.setValue(null);
  }

  save(): void {
    this.updateGroups();
    this.updateUsers();
  }

  updateGroups(): void {
    var groupIds = this.groups.map(group => group.id);

    this.originalGroupIds.forEach(originalGroupId => {
      if (!groupIds.includes(originalGroupId)) {
        this.albumClient.unshareAlbumWithGroup(this.album.id, originalGroupId).subscribe(
          () => {
            this.snackbarService.openSuccess("Album share update successful");
          },
          (error) => {
            this.snackbarService.openError(error.detail);
          }
        );
      }
    })

    groupIds.forEach(newGroupId => {
      if (!this.originalGroupIds.includes(newGroupId)) {
        this.albumClient.shareAlbumWithGroup(this.album.id, newGroupId).subscribe(
          () => {
            this.snackbarService.openSuccess("Album share update successful");
          },
          (error) => {
            this.snackbarService.openError(error.detail);
          }
        );
      }
    });
  }

  updateUsers(): void {
    var userIds = this.users.map(user => user.id);

    this.originalUserIds.forEach(originalUserId => {
      if (!userIds.includes(originalUserId)) {
        this.albumClient.unshareAlbumWithUser(this.album.id, originalUserId).subscribe(
          () => {
            this.snackbarService.openSuccess("Album share update successful");
          },
          (error) => {
            this.snackbarService.openError(error.detail);
          }
        );
      }
    })

    userIds.forEach(newUserId => {
      if (!this.originalUserIds.includes(newUserId)) {
        this.albumClient.shareAlbumWithUser(this.album.id, newUserId).subscribe(
          () => {
            this.snackbarService.openSuccess("Album share update successful");
          },
          (error) => {
            this.snackbarService.openError(error.detail);
          }
        );
      }
    });
  }

  private _filterGroups(groupName: string): GroupDto[] {
    return this.allGroups.filter(g => g.name.toLowerCase().indexOf(groupName.toLowerCase()) === 0);
  }

  private _filterUsers(userName: string): UserDto[] {
    return this.allUsers.filter(u => u.userName.toLowerCase().indexOf(userName.toLowerCase()) === 0);
  }

  publicShare(): void {
    this.copyToClipboard(
      window.location.protocol +
      '//' +
      window.location.host +
      '/albums/' +
      this.album.path
    );
    this.snackbarService.openSuccess('Album link copied to clipboard');
  }

  copyToClipboard(text: string): void {
    const selBox = document.createElement('textarea');
    selBox.style.position = 'fixed';
    selBox.style.left = '0';
    selBox.style.top = '0';
    selBox.style.opacity = '0';
    selBox.value = text;
    document.body.appendChild(selBox);
    selBox.focus();
    selBox.select();
    document.execCommand('copy');
    document.body.removeChild(selBox);
  }
}
