import { Component, OnInit } from '@angular/core';
import { AlbumClient, AlbumDto, ImageClient } from '../api/app.generated';
import { ModalService } from '../core/modal/modal.service';
import { SnackbarService } from '../core/snackbar/snackbar.service';

@Component({
  selector: 'app-albums',
  templateUrl: './albums.component.html',
  styleUrls: ['./albums.component.scss'],
})
export class AlbumsComponent implements OnInit {
  searchValue: String = '';
  albums: AlbumDto[];
  filteredAlbums: AlbumDto[];

  constructor(
    private modalService: ModalService,
    private albumClient: AlbumClient,
    private snackbarService: SnackbarService
  ) { }

  ngOnInit(): void {
    this.albumClient.getAlbums().subscribe(
      (r) => {
        this.albums = r;
        this.filteredAlbums = this.albums;
      },
      (error) => {
        this.snackbarService.openError(error.detail);
      }
    );
  }

  onSearch(value: String) {
    this.searchValue = value;
    this.filteredAlbums = this.albums.filter(album => album.name.toLowerCase().includes(this.searchValue.toLowerCase()));
  }

  createAlbum(): void {
    this.modalService
      .textinput('Album name', 'Create album', true, 'Create')
      .afterClosed()
      .subscribe((albumName) => {
        if (albumName) {
          this.albumClient.createAlbum(albumName).subscribe(
            (r) => {
              this.albums.push(r);
              this.snackbarService.openSuccess('Album creation successful');
            },
            (error) => {
              this.snackbarService.openError(error.detail);
            }
          );
        }
      });
  }

  onAlbumDeleted(albumId: number): void {
    this.albums = this.albums.filter((a) => a.id != albumId);
  }
}
