#!/usr/bin/env bash

BASE_PATH='./src/app/clients'

gen_student () {
  swagger-codegen generate -i http://localhost:5000/swagger/student/swagger.json -o "${BASE_PATH}/student" -l typescript-angular
}

gen_teacher () {
  swagger-codegen generate -i http://localhost:5000/swagger/teacher/swagger.json -o "${BASE_PATH}/teacher" -l typescript-angular
}

gen_common () {
  swagger-codegen generate -i http://localhost:5000/swagger/common/swagger.json -o "${BASE_PATH}/common" -l typescript-angular
}

gen_all() {
  echo 'generating student client...'
  gen_student
  echo 'generating teacher client...'
  gen_teacher
  echo 'generating common client...'
  gen_common
  echo 'FINISHED'
}


if [[ $# -eq 0 ]]; then
    gen_all
elif [[ $# -eq 1 ]]; then
         if [[ $1 == 'student' ]]; then
            gen_student
         elif [[ $1 == 'teacher' ]]; then
            gen_teacher
         elif [[ $1 == 'common' ]]; then
            gen_common
         else
            echo 'Wrong argument'
         fi
else
  echo 'Wrong argument'
fi
