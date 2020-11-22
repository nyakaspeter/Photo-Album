import { Component, OnInit } from '@angular/core';
import { AlbumClient, AlbumDto } from '../api/app.generated';
import { ModalService } from '../core/modal/modal.service';
import { SnackbarService } from '../core/snackbar/snackbar.service';

@Component({
  selector: 'app-albums-shared',
  templateUrl: './albums-shared.component.html',
  styleUrls: ['./albums-shared.component.scss'],
})
export class AlbumsSharedComponent implements OnInit {
  albums: AlbumDto[];

  constructor(
    private modalService: ModalService,
    private albumClient: AlbumClient,
    private snackbarService: SnackbarService
  ) {}

  ngOnInit(): void {
    this.albumClient.getSharedAlbums().subscribe(
      (r) => {
        this.albums = r;
      },
      (error) => {
        this.snackbarService.openError(error.detail);
      }
    );
  }
}
