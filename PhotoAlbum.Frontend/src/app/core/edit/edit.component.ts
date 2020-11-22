import { Component, OnInit } from '@angular/core';
import { ImageDto } from 'src/app/api/app.generated';

@Component({
  selector: 'app-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.scss'],
})
export class EditComponent implements OnInit {
  public image: ImageDto;

  constructor() {}

  ngOnInit(): void {}
}
