import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AlbumClient, AlbumDto, ImageClient } from 'src/app/api/app.generated';
import { ModalService } from '../modal/modal.service';
import { SnackbarService } from '../snackbar/snackbar.service';

@Component({
  selector: 'app-share',
  templateUrl: './share.component.html',
  styleUrls: ['./share.component.scss'],
})
export class ShareComponent implements OnInit {
  public album: AlbumDto;

  constructor(
    private modalService: ModalService,
    private imageClient: ImageClient,
    private albumClient: AlbumClient,
    private snackbarService: SnackbarService,
    public dialog: MatDialog
  ) {}

  ngOnInit(): void {}

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
