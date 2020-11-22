import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AlbumClient, AlbumDto } from '../api/app.generated';
import { SnackbarService } from '../core/snackbar/snackbar.service';

@Component({
  selector: 'app-albums-public',
  templateUrl: './albums-public.component.html',
  styleUrls: ['./albums-public.component.scss'],
})
export class AlbumsPublicComponent implements OnInit {
  album: AlbumDto;

  constructor(
    private route: ActivatedRoute,
    private albumClient: AlbumClient,
    private snackbarService: SnackbarService
  ) {}

  ngOnInit(): void {
    const albumPath = this.route.snapshot.paramMap.get('path');

    this.albumClient.getPublicAlbum(albumPath).subscribe(
      (r) => {
        this.album = r;
      },
      (error) => {
        this.snackbarService.openError(error.detail);
      }
    );
  }
}
