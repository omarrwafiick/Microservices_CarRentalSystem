import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CurrentPickupComponent } from './current-pickup.component';

describe('CurrentPickupComponent', () => {
  let component: CurrentPickupComponent;
  let fixture: ComponentFixture<CurrentPickupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CurrentPickupComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CurrentPickupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
