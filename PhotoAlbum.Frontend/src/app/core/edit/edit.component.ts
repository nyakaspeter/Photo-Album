import { Component, OnInit } from '@angular/core';
import { MatChipInputEvent } from '@angular/material/chips';
import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { ImageClient, ImageDto } from 'src/app/api/app.generated';
import { SnackbarService } from '../snackbar/snackbar.service';

@Component({
  selector: 'app-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.scss'],
})
export class EditComponent implements OnInit {
  readonly separatorKeysCodes: number[] = [ENTER, COMMA];

  public image: ImageDto;

  extension: string;
  fileName: string;
  location: string;
  tags: string[] = [];

  constructor(
    private imageClient: ImageClient,
    private snackbarService: SnackbarService,
  ) { }

  ngOnInit(): void {
    this.extension = this.image.fileName.substr(this.image.fileName.lastIndexOf('.') + 1);
    this.fileName = this.image.fileName.split('.').slice(0, -1).join('.');
    this.location = this.image.location;
    this.tags = this.image.tags;
  }

  addTag(event: MatChipInputEvent): void {
    const input = event.input;
    const value = event.value;

    if ((value || '').trim()) {
      this.tags.push(value.trim());
    }

    if (input) {
      input.value = '';
    }
  }

  removeTag(tag: string): void {
    const index = this.tags.indexOf(tag);

    if (index >= 0) {
      this.tags.splice(index, 1);
    }
  }

  save(): void {
    var editImage = this.image;

    editImage.fileName = this.fileName + "." + this.extension;
    editImage.location = this.location;
    editImage.tags = this.tags;

    this.imageClient.editImage(editImage).subscribe(
      (r) => {
        this.image = r;
        this.snackbarService.openSuccess('Image edit successful');
      },
      (error) => {
        this.snackbarService.openError(error.detail);
      }
    );
  }
}
