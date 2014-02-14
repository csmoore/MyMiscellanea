/*
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *    http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#include <QCoreApplication>
#include <QFile>
#include <QFileInfo>
#include <QStringList>

#include "imagehistogram.h"

int main(int argc, char *argv[])
{
  QCoreApplication a(argc, argv);

  QStringList args = a.arguments();

  QString image1, image2;

  if (args.count() <= 2)
  {
    // TODO: Put your own files here for no-args test
    image1 = "C:/Test/CASE1.png";
    image2 = "C:/Test/CASE2.png";
  }
  else
  {
    image1 = args[1];
    image2 = args[2];
  }

  QFileInfo fileInfo1(image1);
  QFileInfo fileInfo2(image2);

  QString fileName1(fileInfo1.fileName());
  QString fileName2(fileInfo2.fileName());

  if (!fileInfo1.exists())
  {
    printf("Image1 doesn't exist: %s, Aborting...\n", qPrintable(image1));
  }
  else if (!fileInfo2.exists())
  {
    printf("Image2 doesn't exist: %s, Aborting...\n", qPrintable(image2));
  }
  else // they both exist so compare
  {
    QImage qimage(image1);
    ImageHistogram ih(qimage);

    QImage qimage2(image2);
    ImageHistogram ih2(qimage2);

    ImageHistogram::MATCH match = ih.CompareTo(&ih2);

    printf("Image1: %s, Image2: %s, Match = %d - ", qPrintable(fileName1),
           qPrintable(fileName2), match);

    if (match == ImageHistogram::PROBABLE_MATCH)
      printf("PROBABLE_MATCH");
    else if (match == ImageHistogram::POSSIBLE_MATCH)
      printf("POSSIBLE_MATCH");
    else
      printf("NO_MATCH");

    printf("\n");
  }

}
