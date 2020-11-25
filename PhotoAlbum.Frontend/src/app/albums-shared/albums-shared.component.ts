import { Component, OnInit } from '@angular/core';
import { AlbumClient, AlbumDto } from '../api/app.generated';
import { SnackbarService } from '../core/snackbar/snackbar.service';

@Component({
  selector: 'app-albums-shared',
  templateUrl: './albums-shared.component.html',
  styleUrls: ['./albums-shared.component.scss'],
})
export class AlbumsSharedComponent implements OnInit {
  searchValue: String = '';
  albums: AlbumDto[];
  filteredAlbums: AlbumDto[];

  constructor(
    private albumClient: AlbumClient,
    private snackbarService: SnackbarService
  ) { }

  ngOnInit(): void {
    this.albumClient.getSharedAlbums().subscribe(
      (r) => {
        this.albums = r;
        this.onSearch(this.searchValue);
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
}
