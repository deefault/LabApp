import { TestBed } from '@angular/core/testing';

import { PreviousLocationService } from './previous-location.service';

describe('PreviousLocationService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: PreviousLocationService = TestBed.get(PreviousLocationService);
    expect(service).toBeTruthy();
  });
});
