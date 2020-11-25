import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { GroupClient, GroupDto } from '../api/app.generated';
import { ModalService } from '../core/modal/modal.service';
import { SnackbarService } from '../core/snackbar/snackbar.service';
import { UsersComponent } from '../core/users/users.component';

@Component({
  selector: 'app-groups',
  templateUrl: './groups.component.html',
  styleUrls: ['./groups.component.scss']
})
export class GroupsComponent implements OnInit {
  searchValue: String = '';
  groups: GroupDto[];
  filteredGroups: GroupDto[];

  constructor(
    private modalService: ModalService,
    private groupClient: GroupClient,
    private snackbarService: SnackbarService,
    public dialog: MatDialog
  ) { }

  ngOnInit(): void {
    this.getGroups();
  }

  onSearch(value: String) {
    this.searchValue = value;
    this.filteredGroups = this.groups.filter(group => group.name.toLowerCase().includes(this.searchValue.toLowerCase()));
  }

  getGroups(): void {
    this.groupClient.getGroups().subscribe(
      (r) => {
        this.groups = r;
        this.onSearch(this.searchValue);
      },
      (error) => {
        this.snackbarService.openError(error.detail);
      }
    );
  }

  createGroup(): void {
    this.modalService
      .textinput('Group name', 'Create group', true, 'Create')
      .afterClosed()
      .subscribe((groupName) => {
        if (groupName) {
          this.groupClient.createGroup(groupName).subscribe(
            (r) => {
              this.groups.push(r);
              this.onSearch(this.searchValue);
              this.snackbarService.openSuccess('Group creation successful');
            },
            (error) => {
              this.snackbarService.openError(error.detail);
            }
          );
        }
      });
  }

  openUsersDialog(group: GroupDto): void {
    let dialogRef = this.dialog.open(UsersComponent, {
      width: '75vw',
      maxHeight: '75vh',
    });
    dialogRef.componentInstance.group = group;

    dialogRef.afterClosed().subscribe(r => {
      if (r) {
        this.getGroups();
      }
    });
  }

  renameGroup(group: GroupDto): void {
    this.modalService
      .textinput('Group name', 'Rename group', true, 'Rename')
      .afterClosed()
      .subscribe((groupmName) => {
        if (groupmName) {
          this.groupClient.renameGroup(group.id, groupmName).subscribe(
            () => {
              group.name = groupmName;
              this.snackbarService.openSuccess('Group rename successful');
            },
            (error) => {
              this.snackbarService.openError(error.detail);
            }
          );
        }
      });
  }

  deleteGroup(groupId: number): void {
    this.modalService
      .alert('Are you sure you want to delete the group?', 'Delete group', true)
      .afterClosed()
      .subscribe((r) => {
        if (r) {
          this.groupClient.deleteGroup(groupId).subscribe(
            () => {
              this.groups = this.groups.filter((a) => a.id != groupId);
              this.onSearch(this.searchValue);
              this.snackbarService.openSuccess('Group delete successful');
            },
            (error) => {
              this.snackbarService.openError(error.detail);
            }
          );
        }
      });
  }
}
