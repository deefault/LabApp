interface ModelStateError {
  property: string;
  errors: string[];
}

export interface ModelState {
  title: string;
  status: number;
  errors: ModelStateError[];
}
