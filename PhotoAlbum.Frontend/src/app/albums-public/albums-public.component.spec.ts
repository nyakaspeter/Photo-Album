import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AlbumsPublicComponent } from './albums-public.component';

describe('AlbumsPublicComponent', () => {
  let component: AlbumsPublicComponent;
  let fixture: ComponentFixture<AlbumsPublicComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AlbumsPublicComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AlbumsPublicComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
