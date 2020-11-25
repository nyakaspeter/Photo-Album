import { Component, Inject, Input, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CommentDto, ImageClient } from 'src/app/api/app.generated';
import { CredentialsService } from '../auth/credentials.service';
import { ModalService } from '../modal/modal.service';
import { SnackbarService } from '../snackbar/snackbar.service';

@Component({
  selector: 'app-comments',
  templateUrl: './comments.component.html',
  styleUrls: ['./comments.component.scss'],
})
export class CommentsComponent implements OnInit {
  public image_id: number;
  public comments: CommentDto[] = [];
  public uploader_id: number;
  comment: string = '';

  user_id: string;

  constructor(
    private modalService: ModalService,
    private credentialsService: CredentialsService,
    private imageClient: ImageClient,
    private snackbarService: SnackbarService,
    public dialogRef: MatDialogRef<CommentsComponent>
  ) { }

  ngOnInit(): void {
    this.user_id = this.credentialsService.credentials.id;
  }

  canDelete(commenter_id: number) {
    return this.user_id === commenter_id.toString() || this.user_id === this.uploader_id.toString();
  }

  onDeleteComment(id: number): void {
    this.modalService
      .alert(
        'Are you sure you want to delete the comment?',
        'Delete comment',
        true,
        'Delete',
        'No'
      )
      .afterClosed()
      .subscribe((r) => {
        if (r) {
          this.imageClient.deleteComment(id).subscribe(
            () => {
              this.comments = this.comments.filter((c) => c.id != id);
              this.snackbarService.openSuccess('Delete successful');
            },
            (error) => this.snackbarService.openError(error.detail)
          );
        }
      });
  }

  postComment(comment: string): void {
    this.imageClient.postComment(this.image_id, comment).subscribe(
      (r) => {
        this.comments.push(r);
        this.snackbarService.openSuccess('Comment successful');
      },
      (error) => {
        this.snackbarService.openError(error.detail);
      }
    );
  }

  onComment(): void {
    if (this.comment.trim()) {
      this.postComment(this.comment);
      this.comment = '';
    }
  }
}
