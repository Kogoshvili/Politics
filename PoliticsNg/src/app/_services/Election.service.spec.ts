/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { ElectionService } from './Election.service';

describe('Service: Election', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ElectionService]
    });
  });

  it('should ...', inject([ElectionService], (service: ElectionService) => {
    expect(service).toBeTruthy();
  }));
});
