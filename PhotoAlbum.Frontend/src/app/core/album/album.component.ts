import {
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
  SimpleChanges,
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import {
  AlbumClient,
  AlbumDto,
  ImageClient,
  ImageDto,
} from 'src/app/api/app.generated';
import { CommentsComponent } from '../comments/comments.component';
import { EditComponent } from '../edit/edit.component';
import { ImageComponent } from '../image/image.component';
import { ModalService } from '../modal/modal.service';
import { ShareComponent } from '../share/share.component';
import { SnackbarService } from '../snackbar/snackbar.service';

@Component({
  selector: 'app-album',
  templateUrl: './album.component.html',
  styleUrls: ['./album.component.scss'],
})
export class AlbumComponent implements OnInit {
  @Input() album: AlbumDto;
  @Input() owned: boolean;

  @Output() albumDeleted: EventEmitter<number> = new EventEmitter();

  titleFilter: String = "";
  locationFilter: String = "";
  tagFilter: String = "";
  filteredImages: ImageDto[];

  constructor(
    private modalService: ModalService,
    private imageClient: ImageClient,
    private albumClient: AlbumClient,
    private snackbarService: SnackbarService,
    public dialog: MatDialog
  ) { }

  ngOnInit(): void { }

  ngOnChanges(changes: SimpleChanges) {
    this.filteredImages = changes.album.currentValue.images;
  }

  titleFilterChanged(value: String): void {
    this.titleFilter = value;
    this.filterImages();
  }

  locationFilterChanged(value: String): void {
    this.locationFilter = value;
    this.filterImages();
  }

  tagFilterChanged(value: String): void {
    this.tagFilter = value;
    this.filterImages();
  }

  filterImages() {
    this.filteredImages = this.album.images.filter((i) =>
      i.fileName.toLowerCase().includes(this.titleFilter.toLowerCase()) &&
      i.location.toLowerCase().includes(this.locationFilter.toLowerCase())
    );

    if (this.tagFilter) {
      this.filteredImages = this.filteredImages.filter((i) =>
        i.tags.includes(this.tagFilter.toLowerCase())
      );
    }
  }

  openImageDialog(image: ImageDto): void {
    let dialogRef = this.dialog.open(ImageComponent, {
      maxHeight: '95vh',
      maxWidth: '95vw',
      panelClass: 'image-dialog',
      backdropClass: 'image-dialog-backdrop',
    });
    dialogRef.componentInstance.image = image;
    dialogRef.componentInstance.owned = this.owned;
    dialogRef.componentInstance.imageDeleted.subscribe((r: number) => {
      this.album.images = this.album.images.filter((i) => i.id != r);
      this.filteredImages = this.filteredImages.filter((i) => i.id != r);
      dialogRef.close();
    });
    dialogRef.componentInstance.closeDialog.subscribe(() => {
      dialogRef.close();
    });
  }

  openCommentsDialog(image: ImageDto): void {
    let dialogRef = this.dialog.open(CommentsComponent, {
      width: '75vw',
      maxHeight: '75vh',
    });
    dialogRef.componentInstance.image_id = image.id;
    dialogRef.componentInstance.comments = image.comments;
  }

  editImage(image: ImageDto): void {
    let dialogRef = this.dialog.open(EditComponent, {
      width: '75vw',
      maxHeight: '75vh',
    });
    dialogRef.componentInstance.image = image;
  }

  deleteImage(imageId: number): void {
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
          this.imageClient.deleteImage(imageId).subscribe(
            () => {
              this.album.images = this.album.images.filter(
                (i) => i.id != imageId
              );
              this.filteredImages = this.filteredImages.filter(
                (i) => i.id != imageId
              );
              this.snackbarService.openSuccess('Delete successful');
            },
            (error) => this.snackbarService.openError(error.detail)
          );
        }
      });
  }

  renameAlbum(): void {
    this.modalService
      .textinput('Album name', 'Rename album', true, 'Rename')
      .afterClosed()
      .subscribe((albumName) => {
        if (albumName) {
          this.albumClient.renameAlbum(this.album.id, albumName).subscribe(
            () => {
              this.album.name = albumName;
              this.snackbarService.openSuccess('Album rename successful');
            },
            (error) => {
              this.snackbarService.openError(error.detail);
            }
          );
        }
      });
  }

  deleteAlbum(): void {
    this.modalService
      .alert('Are you sure you want to delete the album?', 'Delete album', true)
      .afterClosed()
      .subscribe((r) => {
        if (r) {
          this.albumClient.deleteAlbum(this.album.id).subscribe(
            () => {
              this.albumDeleted.emit(this.album.id);
              this.snackbarService.openSuccess('Album delete successful');
            },
            (error) => {
              this.snackbarService.openError(error.detail);
            }
          );
        }
      });
  }

  uploadImages(files): void {
    if (files.length !== 0) {
      for (let i = 0; i < files.length; i++) {
        const file = files[i] as File;
        let fileParameter = {
          fileName: file.name,
          data: file,
        };

        this.imageClient.uploadImage(this.album.id, fileParameter).subscribe(
          (r) => {
            this.album.images.push(r);
          },
          (error) => {
            this.snackbarService.openError(error.detail);
          }
        );
      }
    }
  }

  shareAlbum(): void {
    let dialogRef = this.dialog.open(ShareComponent, {
      width: '75vw',
      maxHeight: '75vh',
    });
    dialogRef.componentInstance.album = this.album;
  }

  shareImage(imagePath: string): void {
    this.copyToClipboard(
      window.location.protocol +
      '//' +
      window.location.host +
      '/albums/' +
      imagePath
    );
    this.snackbarService.openSuccess('Image link copied to clipboard');
  }

  downloadAlbum(): void {
    this.albumClient.downloadAlbum(this.album.id).subscribe(
      (r) => {
        this.downloadLink(
          window.location.protocol +
          '//' +
          window.location.host +
          '/albums/.zips/' +
          r
        );
      },
      (error) => {
        this.snackbarService.openError(error.detail);
      }
    );
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

  downloadLink(url: string): void {
    const link = document.createElement('a');
    link.setAttribute('type', 'hidden');
    link.setAttribute('href', url);
    link.setAttribute('download', 'album.zip');
    document.body.appendChild(link);
    link.click();
    link.remove();
  }
}
