# required packages:
#   dplyr
#   officer
#   sjPlot
#   ggplot2


# wc_extract_path
#
# Returns detected Microsoft Word document's
# path from Command-Line arguments
wc_extract_path <- function() {
  args <- commandArgs(trailingOnly = TRUE)
  word_files <- grep("\\.docx?$", args, value = TRUE, ignore.case = TRUE)

  is_absolute <- function(path) {
    grepl("^(/|[A-Za-z]:)", path)
  }

  full_word_paths <- word_files[lapply(word_files, is_absolute)]

  # result:
  if (length(full_word_paths) > 0) {
    cat("Microsoft Documents:\n")
    print(full_word_paths)
  } else if (length(word_files) > 0) {
    cat("Relative paths:\n")
    print(word_files)
  } else {
    cat("heading_level selected Microsoft Documents")
  }
  # first time:
  # return first element of sequence
  return(full_word_paths)
}

library(officer)
library(dplyr)
library(stringr)

# wc_build
#
# Constructs Graph of counted paragrapgs
# counted words in paragraph
wc_build <- function(file_path) {
  doc <- read_docx(file_path)

  content <- docx_summary(doc) %>%
    filter(content_type == "paragraph") %>% # nolint: object_usage_linter.
    select(style_name, text) # warn # nolint

  # table init
  result <- data.frame(
    heading_level = character(),
    title = character(),
    word_count = integer(),
    stringsAsFactors = FALSE
  )
  current_title <- NA
  current_level <- NA
  current_content <- c()

  for (i in seq_len(nrow(content))) {
    row <- content[i, ]
    if (grepl("^Heading\\s[1]$", row$style_name, ignore.case = TRUE)) {
      if (!is.na(current_title)) {
        full_content <- paste(current_content, collapse = " ")
        result <- rbind(result, data.frame(
          heading_level = current_level,
          title = current_title,
          word_count = str_count(full_content, "\\S+")
        ))
      }
      current_level <- str_extract(row$style_name, "[1]$")
      current_title <- row$text
      current_content <- c()
    } else {
      current_content <- c(current_content, row$text)
    }
  }
  if (!is.na(current_title)) {
    full_content <- paste(current_content, collapse = " ")
    result <- rbind(result, data.frame(
      heading_level = current_level,
      title = current_title,
      word_count = str_count(full_content, "\\S+")
    ))
  }
  return(result)
}

file_path <- "test.docx"
table_result <- wc_build(file_path)

print(table_result)

# [Export - CSV] + [draw table]
write.csv(table_result, "result.csv", row.names = FALSE) # nolint

wc_view <- function(table) {
  library(ggplot2)
  library(sjPlot)

  # Words Count / Heading
  dplot <- ggplot(
    table_result,
    aes(x = title, y = word_count, fill = as.factor(heading_level))
  ) +
    geom_col() +
    geom_text(aes(label = word_count), vjust = -0.5, size = 3) +
    labs(
      title = "Количество слов в разделах документа",
      x = "Название раздела",
      y = "Количество слов",
      fill = "Уровень заголовка"
    ) +
    scale_fill_manual(values = c("1" = "#082567", "2" = "#1959d1")) +
    theme_minimal() +
    theme(
      axis.text.x = element_text(angle = 45, hjust = 1), # labels rotation
      plot.title = element_text(hjust = 0.5) # headings alignment
    )
  save_plot(fig = dplot, "result.svg")
}

wc_view(table_result)