import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AlbumsSharedComponent } from './albums-shared.component';

describe('AlbumsSharedComponent', () => {
  let component: AlbumsSharedComponent;
  let fixture: ComponentFixture<AlbumsSharedComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AlbumsSharedComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AlbumsSharedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
