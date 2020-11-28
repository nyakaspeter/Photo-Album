import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ImageClient, ImageDto } from 'src/app/api/app.generated';
import { CredentialsService } from '../auth/credentials.service';
import { CommentsComponent } from '../comments/comments.component';
import { EditComponent } from '../edit/edit.component';
import { ModalService } from '../modal/modal.service';
import { SnackbarService } from '../snackbar/snackbar.service';

@Component({
  selector: 'app-image',
  templateUrl: './image.component.html',
  styleUrls: ['./image.component.scss'],
})
export class ImageComponent implements OnInit {
  @Input() public image: ImageDto;
  @Input() public owned: boolean;

  @Output() imageDeleted: EventEmitter<number> = new EventEmitter();
  @Output() closeDialog: EventEmitter<void> = new EventEmitter();

  constructor(
    private snackbarService: SnackbarService,
    private credentialsService: CredentialsService,
    private imageClient: ImageClient,
    private modalService: ModalService,
    public dialog: MatDialog
  ) {}

  ngOnInit(): void {}

  openCommentsDialog(): void {
    let dialogRef = this.dialog.open(CommentsComponent, {
      width: '75vw',
      maxHeight: '75vh',
    });
    dialogRef.componentInstance.image_id = this.image.id;
    dialogRef.componentInstance.comments = this.image.comments;
    dialogRef.componentInstance.uploader_id = this.image.uploader.id;
  }

  editImage(): void {
    let dialogRef = this.dialog.open(EditComponent, {
      width: '75vw',
      maxHeight: '75vh',
    });
    dialogRef.componentInstance.image = this.image;
  }

  shareImage(): void {
    this.copyToClipboard(
      window.location.protocol +
        '//' +
        window.location.host +
        '/albums/' +
        this.image.path
    );
    this.snackbarService.openSuccess('Image link copied to clipboard');
  }

  deleteImage(): void {
    this.modalService
      .alert(
        'Are you sure you want to delete the image?',
        'Delete image',
        true,
        'Delete',
        'No'
      )
      .afterClosed()
      .subscribe((r) => {
        if (r) {
          this.imageClient.deleteImage(this.image.id).subscribe(
            () => {
              this.imageDeleted.emit(this.image.id);
              this.snackbarService.openSuccess('Delete successful');
            },
            (error) => this.snackbarService.openError(error.detail)
          );
        }
      });
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
